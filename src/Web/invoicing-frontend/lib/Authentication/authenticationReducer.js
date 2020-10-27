export const SET_USER = "SET_USER";

export function authenticationReducer(state, action) {
  if (action.type === SET_USER) {
    return { ...state, isLoggedIn: !!action.payload, user: action.payload };
  }

  return state;
}
