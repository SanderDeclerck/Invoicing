import React, { createContext, useEffect } from "react";
import { UserManager } from "oidc-client";
import { authenticationReducer, SET_USER } from "./authenticationReducer";
import getSiteConfiguration from "../../shared/configuration/siteConfiguration";
import { useThunkReducer } from "../../shared/hooks/useThunkReducer";

export const AuthenticationContext = createContext();
console.log(1);
var { identityService } = getSiteConfiguration();

const initialState = {
  isLoggedIn: false,
  user: {},
  userManager: new UserManager({
    authority: identityService.authority,
    client_id: "invoicing.frontend",
    redirect_uri: identityService.redirectUri,
    response_type: "code",
    response_mode: "query",
    scope: "openid profile customer.api",
    post_logout_redirect_uri: identityService.postLogoutRedirectUri,
  }),
};

export function AuthenticationProvider({ children }) {
  var [authentication, dispatch] = useThunkReducer(
    authenticationReducer,
    initialState
  );

  useEffect(initialize, []);

  function initialize() {
    if (!authentication.isLoggedIn) {
      dispatch(setUserFromUserManager);
    }
  }

  async function setUserFromUserManager(dispatch) {
    var user = await authentication.userManager.getUser();
    if (user) {
      dispatch({
        type: SET_USER,
        payload: await authentication.userManager.getUser(),
      });
    }
  }

  function signIn() {
    return authentication.userManager.signinRedirect();
  }

  function signOut() {
    return authentication.userManager.signoutRedirect();
  }

  async function handleCallback() {
    await authentication.userManager.signinRedirectCallback();
    dispatch(setUserFromUserManager);
  }

  var value = { authentication, signIn, signOut, handleCallback };

  return (
    <AuthenticationContext.Provider value={value}>
      {children}
    </AuthenticationContext.Provider>
  );
}
