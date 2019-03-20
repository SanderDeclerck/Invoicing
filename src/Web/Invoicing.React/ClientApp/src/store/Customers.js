const requestCustomersType = "REQUEST_CUSTOMERS";
const receiveCustomersType = "RECEIVE_CUSTOMERS";
const initialState = { customers: [], isLoading: false };

export const actionCreators = {
  requestCustomers: () => async (dispatch, getState) => {
    dispatch({ type: requestCustomersType });

    const url = 'https://localhost:44339/api/v1/Customers';
    const response = await fetch(url);
    const customers = await response.json();

    dispatch({ type: receiveCustomersType, customers });

  }
};

export const reducer = (state, action) => {
  state = state || initialState;
  
  if (action.type === requestCustomersType) {
    return {
      ...state,
      isLoading: true
    };
  }

  if (action.type === receiveCustomersType) {
    console.log(action);
    return {
      ...state,
      isLoading: false,
      customers: action.customers
    };
  }

  console.log(state);
  return state;
}