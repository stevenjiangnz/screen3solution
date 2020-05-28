import RequestHelper from "../util/RequestHelper";

export class TickerService {
  async getTickerList(code, type = "day", start = 0, end = 0) {
    const tickers = [];
    const req = new RequestHelper().getIntance();
    const resp = await req.get(`ticker/${code}`);

    resp.data.forEach((t) => {
      tickers.push([t.p_Stamp, t.o, t.h, t.l, t.c]);
    });
    console.log(`${code} - ${tickers.length}`);
    return tickers;
  }
}

export default TickerService;
