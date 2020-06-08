import React, { Component } from "react";
import screenMACDWilliam from "../../features/screen_macd_willaim";
import ScreenService from "../../service/ScreenService";

export class ScreenInput extends Component {
  screenService;
  constructor(props) {
    super(props);

    this.screenService = new ScreenService();
    this.state = {
      screenRequest: JSON.stringify(screenMACDWilliam, undefined, 4),
    };
  }

  state = {};

  updateScreenRequest = (e) => {
    this.setState({
      screenRequest: e.target.value,
    });
  };

  submitScreenRequest = () => {
    const screenRequest = JSON.parse(this.state.screenRequest);
    const requestTasks = [];

    screenRequest.stocks.forEach((stock) => {
      requestTasks.push(
        this.screenService.SubmitScreen_MACD_William(
          stock,
          screenRequest.options,
          screenRequest.start,
          screenRequest.end
        )
      );
    });

    console.log(`about to send ${requestTasks.length} request in one go...`);
    Promise.all(requestTasks).then((resps) => {
      var matchList = [];

      resps.forEach((resp) => {
        matchList = matchList.concat(resp.data);
      });

      console.log("returned: ", matchList.length);
    });
  };

  resetScreenInput = () => {
    this.setState({
      screenRequest: JSON.stringify(screenMACDWilliam, undefined, 4),
    });
  };

  render() {
    return (
      <div style={{ marginTop: 10, marginBottom: 10 }}>
        <div className="form-group">
          <textarea
            className="form-control screen-request-input"
            value={this.state.screenRequest}
            onChange={this.updateScreenRequest}
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
