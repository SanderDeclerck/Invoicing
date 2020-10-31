import { useNavigate } from "@reach/router";
import React, { useContext, useEffect } from "react";
import { AuthenticationContext } from "./authenticationContext";

function Callback() {
  const { authentication, handleCallback } = useContext(AuthenticationContext);
  const navigate = useNavigate();

  useEffect(function init() {
    processCallback();
  }, []);

  useEffect(
    function authenticationChanged() {
      if (authentication.isLoggedIn) {
        navigate("/");
      }
    },
    [authentication]
  );

  async function processCallback() {
    await handleCallback();
  }

  return (
    <>
      <h1>Callback</h1>
      <div>Processing...</div>
    </>
  );
}

export default Callback;
