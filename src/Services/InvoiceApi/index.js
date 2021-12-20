import container from './infrastructure/config/container.js';
import createServer from './infrastructure/webserver/fastify.js'

var server = createServer(container.cradle);

async function start() {
  try {
    await server.listen(3000);
  } catch (err) {
    server.log.error(err);
    process.exit(1);
  }
}
start();
