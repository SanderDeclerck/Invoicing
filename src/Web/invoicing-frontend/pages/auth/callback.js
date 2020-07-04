import { useEffect, useContext } from "react";
import { useRouter } from "next/router";
import { AuthenticationContext } from "../../lib/Authentication/AuthenticationContext";

const Callback = () => {
  const router = useRouter();
  const { authentication, handleCallback } = useContext(AuthenticationContext);

  useEffect(() => {
    if (!authentication.isLoggedIn && authentication.isReady) {
      handleCallback();
    }
    if (authentication.isLoggedIn) {
      router.push("/");
    }
  }, [authentication.isLoggedIn, authentication.isReady]);

  return (
    <>
      <h1>Callback</h1>
      <div>Processing...</div>
    </>
  );
};

export default Callback;
