import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Link from "next/link";

const ActionButton = ({ name, icon, url }) => (
  <>
    <Link href={url}>
      <a className="action-button">
        <FontAwesomeIcon icon={["fal", icon]} size="2x" />
        <div>{name}</div>
      </a>
    </Link>
  </>
);

const PageHeader = () => {
  return (
    <>
      <header>
        <nav className="submenu">
          <div className="logo">
            <h1 onClick={() => console.log("lol")}>SD INVOICING</h1>
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
            <h3>Logged in as</h3>
            <div>
              <a href="#" className="account-user">
                Sander Declerck
              </a>
            </div>
            <div>
              <a href="#" className="account-tenant">
                SD Software Bv
              </a>
            </div>
          </div>
        </nav>
      </header>
    </>
  );
};

export default PageHeader;
