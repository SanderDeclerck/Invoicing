import "../styles.scss";
import { library } from "@fortawesome/fontawesome-svg-core";
import {
  faAddressBook,
  faFileInvoiceDollar,
  faCertificate,
  faCoffee,
  faSignOutAlt,
  faSignInAlt,
} from "@fortawesome/pro-light-svg-icons";
import PageHeader from "../components/global/PageHeader";
import { AuthenticationProvider } from "../lib/Authentication/AuthenticationContext";

const setupFontawesome = () => {
  library.add(
    faAddressBook,
    faFileInvoiceDollar,
    faCertificate,
    faCoffee,
    faSignOutAlt,
    faSignInAlt
  );
};

// This default export is required in a new `pages/_app.js` file.
export default function MyApp({ Component, pageProps }) {
  setupFontawesome();
  return (
    <AuthenticationProvider>
      <PageHeader />
      <div className="page-content">
        <Component {...pageProps} />
      </div>
    </AuthenticationProvider>
  );
}
