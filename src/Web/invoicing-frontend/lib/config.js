function getSiteConfiguration() {
  return {
    identityService: {
      authority: process.env.OIDC_AUTHORITY,
      redirectUri: process.env.OIDC_CALLBACK,
      postLogoutRedirectUri: process.env.OIDC_POST_LOGOUT_REDIRECT,
    },
    customerService: {
      baseUri: process.env.CUSTOMER_SERVICE_API_BASEURL,
    },
  };
}

export default getSiteConfiguration;
