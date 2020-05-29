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
      convertedIndicators.push([ind.p_Stamp, ind.v ? ind.v : 0]);
    });

    return convertedIndicators;
  }
}

export default TickerHelper;
