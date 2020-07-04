export default function handler(req, res) {
  const oidcAuthority = process.env.OIDC_AUTHORITY;
  const callback = process.env.OIDC_CALLBACK;
  const postLogoutRedirect = process.env.OIDC_POST_LOGOUT_REDIRECT;

  const oidcConfig = {
    authority: oidcAuthority,
    client_id: "invoicing.frontend",
    redirect_uri: callback,
    response_type: "code",
    response_mode: "query",
    scope: "openid profile customer.api",
    post_logout_redirect_uri: postLogoutRedirect,
  };

  res.status(200).json(oidcConfig);
}
