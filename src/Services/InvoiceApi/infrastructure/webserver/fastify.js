import Fastify from 'fastify';

function createServer({ routes }) {
  var server = Fastify({
    logger: true
  });

  for (var i = 0; i < routes.length; i++) {
    server.route(routes[i]);
  }

  return server;
}

export default createServer;
