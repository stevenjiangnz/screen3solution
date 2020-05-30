import RequestHelper from "../util/RequestHelper";

export class IndictorService {
  getSMA(code, period = 20, type = "day", start = 0, end = 0) {
    const req = new RequestHelper().getIntance();
    return req.get(
      `indicator/sma/${code}?period=${period}&type=${type}&start=${start}&end=${end}`
    );
  }

  getEMA(code, period = 20, type = "day", start = 0, end = 0) {
    const req = new RequestHelper().getIntance();
    return req.get(
      `indicator/ema/${code}?period=${period}&type=${type}&start=${start}&end=${end}`
    );
  }

  getBB(code, type = "day", start = 0, end = 0) {
    const req = new RequestHelper().getIntance();
    return req.get(
      `indicator/bb/${code}?type=${type}&start=${start}&end=${end}`
    );
  }
}

export default IndictorService;
