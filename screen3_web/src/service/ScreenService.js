import RequestHelper from "../util/RequestHelper";

export class ScreenService {
  static stockList = [];

  SubmitScreen_MACD_William(code, options, start = 0, end = 0) {
    const req = new RequestHelper().getIntance();
    return req.post(
      `screen/macd_william/${code}?start=${start}&end=${end}`,
      options
    );
  }
}

export default ScreenService;
