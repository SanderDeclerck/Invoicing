import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/Customers';

class Customers extends Component {
  componentDidMount() {
    this.ensureDataFetched();
  }

  ensureDataFetched() {
    this.props.requestCustomers();
  }

  render() {
    return (
      <div>
        <h1>Klantenoverzicht</h1>
        {renderCustomersTable(this.props)}
      </div>
    );
  }
}

function renderCustomersTable(props) {
  return (
    <div className='table-responsive'>
      <table className='table table-striped'>
        <thead>
          <tr>
            <th>Voornaam</th>
            <th>Famillienaam</th>
            <th>Firma</th>
            <th>Telefoon</th>
            <th>E-mail</th>
          </tr>
        </thead>
        <tbody>
          {props.customers.map(customer =>
            <tr>
              <td>{customer.firstName}</td>
              <td>{customer.lastName}</td>
              <td>{customer.companyName}</td>
              <td>{customer.phoneNumber}</td>
              <td>{customer.emailAddress}</td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}

export default connect(
  state => state.customers,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(Customers);