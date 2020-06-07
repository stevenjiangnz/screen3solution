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

  getWilliamR(code, type = "day", start = 0, end = 0) {
    const req = new RequestHelper().getIntance();
    return req.get(
      `indicator/williamr/${code}?type=${type}&start=${start}&end=${end}`
    );
  }

  getRSI(code, type = "day", start = 0, end = 0) {
    const req = new RequestHelper().getIntance();
    return req.get(
      `indicator/rsi/${code}?type=${type}&start=${start}&end=${end}`
    );
  }

  getStochastic(code, type = "day", start = 0, end = 0) {
    const req = new RequestHelper().getIntance();
    return req.get(
      `indicator/stochastic/${code}?type=${type}&start=${start}&end=${end}`
    );
  }

  getMACD(code, type = "day", start = 0, end = 0) {
    const req = new RequestHelper().getIntance();
    return req.get(
      `indicator/macd/${code}?type=${type}&start=${start}&end=${end}`
    );
  }

  getADX(code, type = "day", start = 0, end = 0) {
    const req = new RequestHelper().getIntance();
    return req.get(
      `indicator/adx/${code}?type=${type}&start=${start}&end=${end}`
    );
  }

  getHeikin(code, type = "day", start = 0, end = 0) {
    const req = new RequestHelper().getIntance();
    return req.get(
      `indicator/heikinasync/${code}?type=${type}&start=${start}&end=${end}`
    );
  }
}

export default IndictorService;
