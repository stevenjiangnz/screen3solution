import React, { Component } from "react";

export class ScreenInput extends Component {
  constructor(props) {
    super(props);
  }

  submitScreenRequest = () => {
    console.log("about to submit request");
  };

  resetScreenInput = () => {
    console.log("about to clear the input");
  };
  render() {
    return (
      <div style={{ marginTop: 10, marginBottom: 10 }}>
        <div className="form-group">
          <textarea
            className="form-control screen-request-input"
            id="screenInput"
            rows="16"
          ></textarea>
        </div>
        <button
          type="submit"
          className="btn btn-primary"
          onClick={this.submitScreenRequest}
        >
          Submit
        </button>
        &nbsp;
        <button
          type="reset"
          className="btn btn-secondary"
          onClick={this.resetScreenInput}
        >
          Cancel
        </button>
      </div>
    );
  }
}

export default ScreenInput;
