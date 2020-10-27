import React from "react";
import ReactDOM from "react-dom";
import { Router } from "@reach/router";
import PageHeader from "./layout/PageHeader";
import Home from "./pages/home";
import Invoice from "./pages/invoice";
import Catalog from "./pages/catalog";
import Customer from "./pages/customer";
import { AuthenticationProvider } from "./lib/Authentication/AuthenticationContext";
import { default as OidcCallback } from "./pages/callback";

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

document.addEventListener("DOMContentLoaded", function initializeApplcation() {
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
