import React, { Component } from "react";
import screenMACDWilliam from "../../features/screen_macd_willaim";
import screenADX from "../../features/screen_adx";
import ScreenService from "../../service/ScreenService";
import AppContext from "../../Context";

export class ScreenInput extends Component {
  context;
  screenService;
  constructor(props) {
    super(props);

    this.screenService = new ScreenService();
    this.state = {
      screenRequest: JSON.stringify(screenMACDWilliam, undefined, 4),
      isLoading: false,
      selectedScreen: "macd_william",
    };
  }

  state = {};

  updateScreenRequest = (e) => {
    this.setState({
      screenRequest: e.target.value,
    });
  };

  onScreenChange = (e) => {
    var defaultRequest;
    console.log("e.target.value: ", e.target.value);
    switch (e.target.value) {
      case "macd_william":
        defaultRequest = JSON.stringify(screenMACDWilliam, undefined, 4);
        break;
      case "adx":
        defaultRequest = JSON.stringify(screenADX, undefined, 4);
        break;
    }

    this.setState({
      screenRequest: defaultRequest,
      selectedScreen: e.target.value,
    });
  };

  submitScreenRequest = () => {
    const screenRequest = JSON.parse(this.state.screenRequest);
    const requestTasks = [];

    this.setState({
      isLoading: true,
    });

    this.context.setScreenResult([]);

    screenRequest.stocks.forEach((stock) => {
      requestTasks.push(
        this.screenService.SubmitScreen(
          stock,
          this.state.selectedScreen,
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
      this.context.setScreenResult(matchList);

      this.setState({
        isLoading: false,
      });
    });
  };

  resetScreenInput = () => {
    this.setState({
      screenRequest: JSON.stringify(screenMACDWilliam, undefined, 4),
    });
  };

  render() {
    return (
      <AppContext.Consumer>
        {(context) => {
          this.context = context;
          return (
            <div style={{ marginTop: 10, marginBottom: 10 }}>
              <div className="form-group">
                <select
                  className="form-control"
                  id="accountSelector"
                  value={this.state.selectedScreen}
                  onChange={this.onScreenChange}
                >
                  <option key="1" value="macd_william">
                    Screen Macd William %R
                  </option>
                  <option key="2" value="adx">
                    Screen ADX
                  </option>
                </select>
              </div>
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
              {this.state.isLoading && (
                <span style={{ marginLeft: 20 }}>Loading... </span>
              )}
            </div>
          );
        }}
      </AppContext.Consumer>
    );
  }
}

export default ScreenInput;
