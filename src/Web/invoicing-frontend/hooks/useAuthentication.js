import { useState, useEffect } from "react";
import { UserManager } from "oidc-client";
import { useFetch } from "use-http";

const useAuthentication = () => {
  var [authentication, setAuthentication] = useState({
    isLoggedIn: false,
    user: null,
  });

  const { data: oidcConfig } = useFetch("/api/auth/config", {}, []);

  useEffect(() => {
    if (!oidcConfig) return;

    const userManager = new UserManager(oidcConfig);

    userManager.getUser().then((data) => {
      if (data) {
        setAuthentication({ ...authentication, isLoggedIn: true, user: data });
      } else {
        setAuthentication({ ...authentication, isLoggedIn: false, user: null });
      }
    });
  }, [oidcConfig]);

  const signIn = () => {
    const userManager = new UserManager(oidcConfig);

    userManager.signinRedirect();
  };

  const signOut = () => {
    const userManager = new UserManager(oidcConfig);

    userManager.signoutRedirect();
  };

  return [authentication, signIn, signOut];
};

export default useAuthentication;
