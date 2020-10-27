import React, { useContext } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Link } from "@reach/router";
import { AuthenticationContext } from "../lib/Authentication/AuthenticationContext";

function ActionButton({ name, icon, url }) {
  return (
    <>
      <Link className="action-button" to={url}>
        <FontAwesomeIcon icon={["fal", icon]} size="2x" />
        <div>{name}</div>
      </Link>
    </>
  );
}

function PageHeader() {
  const { authentication, signIn, signOut } = useContext(AuthenticationContext);

  return (
    <>
      <header>
        <nav className="submenu">
          <div className="logo">
            <h1>SD INVOICING</h1>
          </div>
          <ActionButton name="Customers" icon="address-book" url="/customer" />
          <ActionButton
            name="Invoices"
            icon="file-invoice-dollar"
            url="/invoice"
          />
          <ActionButton name="Catalog" icon="certificate" url="/catalog" />
        </nav>
        <nav className="submenu">
          <div className="account">
            {authentication.isLoggedIn ? (
              <>
                <h3>Logged in as</h3>
                <div>
                  <span className="account-user">
                    {`${authentication.user.profile.preferred_username} / ${authentication.user.profile.tenant_name}`}
                  </span>
                </div>
                <div>
                  <span className="account-tenant"></span>
                </div>
                <div>
                  <a
                    href="#"
                    className="account-user"
                    onClick={(e) => {
                      e.preventDefault();
                      signOut();
                    }}
                  >
                    Log out{" "}
                    <FontAwesomeIcon icon={["fal", "sign-out-alt"]} size="xs" />
                  </a>
                </div>
              </>
            ) : (
              <>
                <h3>Not logged in</h3>
                <a
                  href="#"
                  className="account-user"
                  onClick={(e) => {
                    e.preventDefault();
                    signIn();
                  }}
                >
                  Log in{" "}
                  <FontAwesomeIcon icon={["fal", "sign-in-alt"]} size="xs" />
                </a>
              </>
            )}
          </div>
        </nav>
      </header>
    </>
  );
}

export default PageHeader;
