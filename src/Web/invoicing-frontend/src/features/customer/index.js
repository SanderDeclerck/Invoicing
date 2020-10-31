import React, { useContext, useEffect, useState } from "react";
import { AuthenticationContext } from "../authentication/authenticationContext";
import getSiteConfiguration from "../../shared/configuration/siteConfiguration";
import { useThunkReducer } from "../../shared/hooks/useThunkReducer";

var { customerService } = getSiteConfiguration();

export default function Customer() {
  var customerState = useCustomerFetch();

  return (
    <>
      <h1>Customers</h1>

      {customerState.isLoading ? (
        "Loading..."
      ) : (
        <table>
          <thead>
            <tr>
              <th>Id</th>
              <th>Name</th>
              <th>Billingaddress</th>
            </tr>
          </thead>
          <tbody>{customerState.customers.map(CustomerRow)}</tbody>
        </table>
      )}

      <CustomerForm />
    </>
  );
}

function CustomerRow(customer) {
  return (
    <tr key={customer.id}>
      <td>{customer.id}</td>
      <td>{customer.name}</td>
      <td>{customer.billingAddress}</td>
    </tr>
  );
}

function CustomerForm() {
  var { authentication } = useContext(AuthenticationContext);
  var [state, setState] = useState({ company: "", vat: "" });

  return (
    <>
      <input
        placeholder="company"
        value={state.company}
        onChange={companyChanged}
      />
      <input placeholder="vat" value={state.vat} onChange={vatChanged} />
      <button onClick={save}>Save</button>
    </>
  );

  function companyChanged(e) {
    setState({ ...state, company: e.target.value });
  }

  function vatChanged(e) {
    setState({ ...state, vat: e.target.value });
  }

  function save(e) {
    var accessToken = authentication.user.access_token;
    var payload = {
      companyName: state.company,
      vatNumber: state.vat,
    };
    fetch(`${customerService.baseUri}/customer/company`, {
      method: "POST",
      headers: {
        Authorization: `Bearer ${accessToken}`,
        "Content-Type": "application/json",
      },
      body: JSON.stringify(payload),
    });
  }
}

function useCustomerFetch() {
  var initialState = {
    customers: [],
    isLoading: true,
  };
  var { authentication } = useContext(AuthenticationContext);
  var [customerState, dispatch] = useThunkReducer(
    customerReducer,
    initialState
  );
  useEffect(authenticationInitialized, [authentication]);

  return customerState;

  function authenticationInitialized() {
    if (!authentication.isLoggedIn) {
      return;
    }

    var accessToken = authentication.user.access_token;

    fetchCustomers(dispatch, accessToken);
  }

  function customerReducer(state, action) {
    if (action.type == "LOADING") {
      return { ...state, customers: [], isLoading: true };
    }

    if (action.type == "RESPONSE_COMPLETE") {
      return { ...state, customers: action.payload, isLoading: false };
    }

    return state;
  }

  async function fetchCustomers(dispatch, accessToken) {
    dispatch({ type: "LOADING" });

    var response = await fetch(`${customerService.baseUri}/api/customers`, {
      headers: { Authorization: `Bearer ${accessToken}` },
    });
    var responseJson = await response.json();
    dispatch({ type: "RESPONSE_COMPLETE", payload: responseJson.customers });
  }
}
