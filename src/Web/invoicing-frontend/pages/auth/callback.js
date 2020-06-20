import { useEffect, useState } from "react";
import { UserManager } from "oidc-client";
import Router, { useRouter } from "next/router";

const Callback = () => {
  const [callbackData, setCallbackData] = useState({ processing: true });
  const router = useRouter();

  useEffect(() => {
    const userManager = new UserManager({ response_mode: "query" });
    userManager
      .signinRedirectCallback()
      .then(() => router.push("/"))
      .catch((error) =>
        setCallbackData({ ...callbackData, processing: false, error })
      );
  }, []);

  return (
    <>
      <h1>Callback</h1>
      {callbackData.processing ? (
        <div>Processing...</div>
      ) : (
        <div>{callbackData.error}</div>
      )}
    </>
  );
};

export default Callback;
