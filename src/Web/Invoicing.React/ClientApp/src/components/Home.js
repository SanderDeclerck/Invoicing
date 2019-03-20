import React from 'react';
import { connect } from 'react-redux';

const Home = props => (
  <div>
    <h1>SD Software</h1>
    <p>Welkom in de facturatie-app van SD Software.</p>
  </div>
);

export default connect()(Home);
