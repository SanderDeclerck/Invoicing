import { useReducer } from "react";
export function useThunkReducer(reducer, initialState) {
  var [state, dispatch] = useReducer(reducer, initialState);

  function thunkDispatch(action) {
    if (typeof action === "function") {
      action(dispatch);
    }

    dispatch(action);
  }

  return [state, thunkDispatch];
}
