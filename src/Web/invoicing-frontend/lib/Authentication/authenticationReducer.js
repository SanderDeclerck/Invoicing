export const SET_USER = "SET_USER";
export const START_INITIALIZE = "START_INITIALIZE";
export const INITIALIZED = "INITIALIZED";
export const authenticationReducer = (state, action) => {
  if (action.type === START_INITIALIZE) {
    return { ...state, isReady: false };
  }

  if (action.type === INITIALIZED) {
    return { ...state, isReady: true, userManager: action.payload };
  }

  if (action.type === SET_USER) {
    return { ...state, isLoggedIn: !!action.payload, user: action.payload };
  }

  return state;
};
