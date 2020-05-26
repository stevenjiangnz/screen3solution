import RequestHelper from "../util/RequestHelper";

export class StockService {
  static stockList = [];

  async getStockList() {
    if (!StockService.stockList || StockService.stockList.length === 0) {
      const req = new RequestHelper().getIntance();
      const resp = await req.get("stock");

      StockService.stockList = resp.data;
    }

    return new Promise((resolve, reject) => {
      resolve({ data: StockService.stockList });
    });
  }
}

export default StockService;
