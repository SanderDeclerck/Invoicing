import React from "react";
import ReactDOM from "react-dom";
import { Router } from "@reach/router";
import PageHeader from "./features/layout/PageHeader";
import Home from "./features/home";
import Invoice from "./features/invoice";
import Catalog from "./features/catalog";
import Customer from "./features/customer";
import { AuthenticationProvider } from "./features/authentication/authenticationContext";
import { default as OidcCallback } from "./features/authentication/callback";

function App() {
  return (
    <AuthenticationProvider>
      <PageHeader />
      <div className="page-content">
        <Router>
          <Home path="/" />
          <Customer path="/customer" />
          <Catalog path="/catalog" />
          <Invoice path="/invoice" />
          <OidcCallback path="/auth/callback" />
        </Router>
      </div>
    </AuthenticationProvider>
  );
}

document.addEventListener("DOMContentLoaded", function initializeApplication() {
  setupFontawesome();
  ReactDOM.render(<App />, document.getElementById("root"));
});

import { library } from "@fortawesome/fontawesome-svg-core";
import {
  faAddressBook,
  faFileInvoiceDollar,
  faCertificate,
  faCoffee,
  faSignOutAlt,
  faSignInAlt,
} from "@fortawesome/pro-light-svg-icons";

function setupFontawesome() {
  library.add(
    faAddressBook,
    faFileInvoiceDollar,
    faCertificate,
    faCoffee,
    faSignOutAlt,
    faSignInAlt
  );
}
