import ChartConfig from "../ChartConfig";
import _ from "lodash";

export class ChartHelper {
  static getIndicatorSetting(ind) {
    return ChartConfig.indicatorSettings[ind];
  }

  static getChartDefaultSettins() {
    return {
      type: "day",
      ema10: false,
      ema20: true,
      ema50: true,
      sma100: false,
      sma200: true,
      bb: true,
      macd: true,
      adx: true,
      heikin: true,
      stochastic: true,
      rsi: false,
      william: true,
    };
  }

  static getOnIndicators(settings, exceptions = ["type"]) {
    const indicators = [];

    Object.keys(settings).forEach((k) => {
      if (!exceptions.includes(k)) {
        if (settings[k]) {
          indicators.push({
            name: k,
            ...ChartHelper.getIndicatorSetting(k),
          });
        }
      }
    });

    return indicators;
  }

  static getGroupUnit() {
    return [
      [
        "week", // unit name
        [1], // allowed multiples
      ],
      ["month", [1, 2, 3, 4, 6]],
    ];
  }
}

export default ChartHelper;
