export class TickerHelper {
  static ConvertTickers(tickers) {
    const convertedTickers = [];

    tickers.forEach((t) => {
      convertedTickers.push([t.p_Stamp, t.o, t.h, t.l, t.c]);
    });

    return convertedTickers;
  }

  static ConvertSingleValueIndicator(Indicator) {
    const convertedIndicators = [];

    Indicator.forEach((ind) => {
      convertedIndicators.push([ind.p_Stamp, ind.v]);
    });

    return convertedIndicators;
  }

  static ConvertBBIndicator(Indicator) {
    const convertedIndicators = {
      high: [],
      mid: [],
      low: [],
    };

    Indicator.forEach((ind) => {
      convertedIndicators.high.push([ind.p_Stamp, ind.high]);
      convertedIndicators.mid.push([ind.p_Stamp, ind.mid]);
      convertedIndicators.low.push([ind.p_Stamp, ind.low]);
    });

    return convertedIndicators;
  }
}

export default TickerHelper;
