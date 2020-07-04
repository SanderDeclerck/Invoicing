import { createContext, useEffect } from "react";
import { UserManager } from "oidc-client";
import { useThunkReducer } from "../useThunkReducer";
import {
  authenticationReducer,
  START_INITIALIZE,
  INITIALIZED,
  SET_USER,
} from "./authenticationReducer";

export const AuthenticationContext = createContext();

const initialState = {
  isLoggedIn: false,
  isReady: false,
  user: {},
  userManager: {},
};

export const AuthenticationProvider = ({ children }) => {
  const [authentication, dispatch] = useThunkReducer(
    authenticationReducer,
    initialState
  );

  useEffect(() => {
    dispatch(async (dispatch) => {
      dispatch({ type: START_INITIALIZE });
      const response = await fetch("/api/auth/config");
      const oidcConfig = await response.json();
      dispatch({ type: INITIALIZED, payload: new UserManager(oidcConfig) });
    });
  }, []);

  useEffect(() => {
    if (authentication.isReady && !authentication.isLoggedIn) {
      dispatch(async (dispatch) => {
        dispatch({
          type: SET_USER,
          payload: await authentication.userManager.getUser(),
        });
      });
    }
  }, [authentication]);

  const signIn = () =>
    authentication.isReady && authentication.userManager.signinRedirect();

  const signOut = () =>
    authentication.isReady && authentication.userManager.signoutRedirect();

  const handleCallback = () => {
    if (!authentication.isReady) {
      return;
    }
    dispatch(async (dispatch) => {
      await authentication.userManager.signinRedirectCallback();
      dispatch({ type: SET_USER, payload: await userManager.getUser() });
    });
  };

  const value = { authentication, signIn, signOut, handleCallback };

  return (
    <AuthenticationContext.Provider value={value}>
      {children}
    </AuthenticationContext.Provider>
  );
};
