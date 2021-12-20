
function routes() {
  return [
    {
      method: 'GET',
      url: '/',
      handler: async (req, res) => { return { a: 1 }; }
    }
  ]
}

export default routes;
