import React, { Component } from "react";
import AppContext from "../../Context";
import Highcharts, { each } from "highcharts/highstock";
import HighchartsReact from "highcharts-react-official";
import TickerService from "../../service/TickerService";
import IndicatorService from "../../service/IndicatorService";
import TickerHelper from "../../util/TickerHelper";
import ChartHelper from "../../util/ChartHelper";

export class StockChart extends Component {
  context;
  chartRef;
  chart;
  tickerService;
  indicatorService;
  groupingUnits;
  chartName;
  tickers;
  tickersOrigin;
  currentChartSettings;
  defaultChartSetting = ChartHelper.getChartDefaultSettins();
  redrawLines = false;
  resetRange = false;
  previousTradeTicker = {};

  ChartPosition = {
    base: 420,
    gap: 10,
    bottom: 20,
    platColor: "#bbb",
  };

  constructor(props) {
    super(props);
    this.chartName = props.name;
    this.tickerService = new TickerService();
    this.indicatorService = new IndicatorService();
    this.chartRef = React.createRef();

    this.groupingUnits = ChartHelper.getGroupUnit();

    this.state = {
      chart: {
        marginRight: 20,
        marginLeft: 10,
      },
      series: [
        {
          type: "candlestick",
          data: [],
          cursor: "pointer",
          groupingUnits: this.groupingUnits,
        },
        {
          name: "volume",
          type: "column",
          lineWidth: 1,
          yAxis: 1,
          groupingUnits: this.groupingUnits,
        },
      ],
      rangeSelector: {
        selected: 2,
      },
      tooltip: {
        enabled: false,
      },
      xAxis: {
        type: "datetime",
        crosshair: {
          label: {
            enabled: true,
            padding: 8,
          },
        },
        opposite: true,
      },
      yAxis: [
        {
          title: {
            text: "OHLC",
          },
          height: 300,
          lineWidth: 1,
          top: 110,
          crosshair: {
            label: {
              enabled: false,
            },
          },
        },
        {
          top: 330,
          height: 80,
          lineWidth: 1,
          offset: -6,
          opposite: false,
        },
      ],
      navigator: {
        height: 25,
        top: 75,
      },
      credits: {
        enabled: false,
      },
    };
  }

  componentDidMount() {
    this.chart = this.chartRef.current.chart;
    this.prepareDrawChart();
  }

  componentDidUpdate(prevProps, prevState) {
    var changeStock = false;
    if (this.props.stock !== prevProps.stock) {
      changeStock = true;
      this.prepareDrawChart();

      this.redrawLines =
        JSON.stringify(this.props.screenResult) !==
        JSON.stringify(prevProps.screenResult);
    }

    if (
      this.context.state.selectedScreenPoint &&
      this.context.state.selectedScreenPoint.p_Stamp &&
      JSON.stringify(this.previousTradeTicker) !==
        JSON.stringify(this.context.state.selectedScreenPoint)
    ) {
      this.previousTradeTicker = Object.assign(
        this.context.state.selectedScreenPoint
      );
      if (!changeStock) {
        this.setScreenRange(this.context.state.selectedScreenPoint);
      }
    }
  }

  setScreenRange = (screenPoint) => {
    const foundIndex = this.tickers.findIndex(
      (ticker) => ticker[0] === screenPoint.p_Stamp
    );
    try {
      var max = this.tickers[foundIndex][0];
      var min =
        foundIndex >= 100
          ? this.tickers[foundIndex - 100][0]
          : this.tickers[0][0];

      this.chart.xAxis[0].setExtremes(min, max);
      this.setTradeTicker(max);
    } catch (err) {
      console.log(err);
    }
  };

  prepareDrawChart = async () => {
    const stock = this.props.stock;
    const indicators = ChartHelper.getOnIndicators(this.currentChartSettings);
    const dataTasks = [];

    if (!stock || !stock.code) {
      return;
    }
    dataTasks.push(
      this.tickerService
        .getTickerList(stock.code, this.currentChartSettings.type)
        .then((value) => {
          this.tickersOrigin = value.data;
          return TickerHelper.ConvertTickers(value.data);
        })
    );

    indicators.forEach((ind) => {
      const param = ind.parameter.split(",");
      switch (param[0]) {
        case "sma":
          dataTasks.push(
            this.indicatorService
              .getSMA(stock.code, param[1], this.currentChartSettings.type)
              .then((value) =>
                TickerHelper.ConvertSingleValueIndicator(value.data)
              )
          );
          break;
        case "ema":
          dataTasks.push(
            this.indicatorService
              .getEMA(stock.code, param[1], this.currentChartSettings.type)
              .then((value) =>
                TickerHelper.ConvertSingleValueIndicator(value.data)
              )
          );
          break;
        case "bb":
          dataTasks.push(
            this.indicatorService.getBB(
              stock.code,
              this.currentChartSettings.type
            )
          );
          break;
        case "macd":
          dataTasks.push(
            this.indicatorService.getMACD(
              stock.code,
              this.currentChartSettings.type
            )
          );
          break;
        case "adx":
          dataTasks.push(
            this.indicatorService.getADX(
              stock.code,
              this.currentChartSettings.type
            )
          );
          break;
        case "heikin":
          dataTasks.push(
            this.indicatorService.getHeikin(
              stock.code,
              this.currentChartSettings.type
            )
          );
          break;
        case "stochastic":
          dataTasks.push(
            this.indicatorService.getStochastic(
              stock.code,
              this.currentChartSettings.type
            )
          );
          break;
        case "rsi":
          dataTasks.push(
            this.indicatorService.getRSI(
              stock.code,
              this.currentChartSettings.type
            )
          );
          break;
        case "william":
          dataTasks.push(
            this.indicatorService.getWilliamR(
              stock.code,
              this.currentChartSettings.type
            )
          );
          break;
        default:
          console.error(`found unknown indicator parameter ${param[0]}`);
      }
    });

    Promise.all([...dataTasks]).then((values) => {
      this.tickers = values[0];

      this.chart.setTitle({ text: `${stock.code} - ${stock.company}` });
      this.chart.series[0].setData(this.tickers);
      this.chart.series[0].name = `${stock.code} - ${this.currentChartSettings.type}`;

      const volumnData = TickerHelper.ConvertVolume(this.tickersOrigin);
      this.chart.series[1].setData(volumnData);

      for (var i = 1; i < values.length; i++) {
        const indSetting = indicators[i - 1];

        switch (indSetting.name) {
          case "bb":
            const bbData = TickerHelper.ConvertBBIndicator(values[i].data);
            this.drawIndicator(
              indSetting.name + "_high",
              bbData.high,
              indSetting
            );
            // this.drawIndicator(
            //   indSetting.name + "_mid",
            //   bbData.mid,
            //   indSetting
            // );
            this.drawIndicator(
              indSetting.name + "_low",
              bbData.low,
              indSetting
            );
            break;
          case "macd":
            const macdData = TickerHelper.ConvertMACDIndicator(values[i].data);
            const macdAxis = this.chart.get(indSetting.yAxisName);
            if (!macdAxis) {
              this.chart.addAxis({
                id: indSetting.yAxisName,
                title: {
                  text: "MACD",
                },
                lineWidth: 1,
                top: this.ChartPosition.base + this.ChartPosition.gap,
                height: indSetting.height,
                plotLines: [
                  {
                    value: 0,
                    color: this.ChartPosition.plotColor,
                    dashStyle: "shortdash",
                    width: 1,
                  },
                ],
                opposite: true,
              });

              this.ChartPosition.base =
                this.ChartPosition.base +
                this.ChartPosition.gap +
                indSetting.height;
            }

            this.drawIndicator(indSetting.name + "_macd", macdData.macd, {
              yAxis: indSetting.yAxisName,
              color: indSetting.colorMacd,
            });
            this.drawIndicator(indSetting.name + "_Signal", macdData.signal, {
              yAxis: indSetting.yAxisName,
              color: indSetting.colorSignal,
            });
            this.drawIndicator(indSetting.name + "_Hist", macdData.hist, {
              yAxis: indSetting.yAxisName,
              color: indSetting.colorHist,
              chartType: "column",
            });
            break;
          case "adx":
            const adxData = TickerHelper.ConvertADXIndicator(values[i].data);
            const adxAxis = this.chart.get(indSetting.yAxisName);
            if (!adxAxis) {
              this.chart.addAxis({
                id: indSetting.yAxisName,
                title: {
                  text: "ADX",
                },
                lineWidth: 1,
                top: this.ChartPosition.base + this.ChartPosition.gap,
                height: indSetting.height,
                labels: {
                  enabled: false,
                },
                opposite: true,
              });

              this.ChartPosition.base =
                this.ChartPosition.base +
                this.ChartPosition.gap +
                indSetting.height;
            }

            this.drawIndicator(indSetting.name + "_adx", adxData.adx, {
              yAxis: indSetting.yAxisName,
              color: indSetting.colorAdx,
            });

            this.drawIndicator(indSetting.name + "_diplus", adxData.di_plus, {
              yAxis: indSetting.yAxisName,
              color: indSetting.colorDiPlus,
            });
            this.drawIndicator(indSetting.name + "_diminus", adxData.di_minus, {
              yAxis: indSetting.yAxisName,
              color: indSetting.colorDiMinus,
            });

            break;
          case "heikin":
            const heikinData = TickerHelper.ConvertHeikinIndicator(
              values[i].data
            );
            const heikinAxis = this.chart.get(indSetting.yAxisName);
            if (!heikinAxis) {
              this.chart.addAxis({
                id: indSetting.yAxisName,
                title: {
                  text: "HEIKIN",
                },
                lineWidth: 1,
                top: this.ChartPosition.base + this.ChartPosition.gap,
                height: indSetting.height,
                labels: {
                  enabled: false,
                },
                opposite: true,
              });

              this.ChartPosition.base =
                this.ChartPosition.base +
                this.ChartPosition.gap +
                indSetting.height;
            }

            this.drawIndicator(indSetting.name + "_k", heikinData, {
              yAxis: indSetting.yAxisName,
              chartType: "candlestick",
            });

            break;
          case "stochastic":
            const stochData = TickerHelper.ConvertStochasticIndicator(
              values[i].data
            );
            const stochAxis = this.chart.get(indSetting.yAxisName);
            if (!stochAxis) {
              this.chart.addAxis({
                id: indSetting.yAxisName,
                title: {
                  text: "STOCHASTIC",
                },
                lineWidth: 1,
                min: 0,
                max: 100,
                top: this.ChartPosition.base + this.ChartPosition.gap,
                height: indSetting.height,
                plotLines: [
                  {
                    value: indSetting.threshold1,
                    color: this.ChartPosition.plotColor,
                    dashStyle: "shortdash",
                    width: 1,
                  },
                  {
                    value: indSetting.threshold2,
                    color: this.ChartPosition.plotColor,
                    dashStyle: "shortdash",
                    width: 1,
                  },
                ],
                labels: {
                  enabled: false,
                },
                opposite: true,
              });

              this.ChartPosition.base =
                this.ChartPosition.base +
                this.ChartPosition.gap +
                indSetting.height;
            }

            this.drawIndicator(indSetting.name + "_k", stochData.k, {
              yAxis: indSetting.yAxisName,
              color: indSetting.colorK,
            });
            this.drawIndicator(indSetting.name + "_d", stochData.d, {
              yAxis: indSetting.yAxisName,
              color: indSetting.colorD,
            });

            break;
          case "rsi":
            const rsiData = TickerHelper.ConvertSingleValueIndicator(
              values[i].data
            );

            const rsiAxis = this.chart.get(indSetting.yAxisName);

            if (!rsiAxis) {
              this.chart.addAxis({
                id: indSetting.yAxisName,
                title: {
                  text: "RSI",
                },
                lineWidth: 1,
                min: 0,
                max: 100,
                top: this.ChartPosition.base + this.ChartPosition.gap,
                height: indSetting.height,
                plotLines: [
                  {
                    value: 30,
                    color: this.ChartPosition.plotColor,
                    dashStyle: "shortdash",
                    width: 1,
                  },
                  {
                    value: 70,
                    color: this.ChartPosition.plotColor,
                    dashStyle: "shortdash",
                    width: 1,
                  },
                ],
                labels: {
                  enabled: false,
                },
                opposite: true,
              });

              this.ChartPosition.base =
                this.ChartPosition.base +
                this.ChartPosition.gap +
                indSetting.height;
            }

            this.drawIndicator(indSetting.name, rsiData, {
              yAxis: indSetting.yAxisName,
              color: indSetting.color,
            });

            break;
          case "william":
            const wrData = TickerHelper.ConvertSingleValueIndicator(
              values[i].data
            );

            const wrAxis = this.chart.get(indSetting.yAxisName);
            if (!wrAxis) {
              this.chart.addAxis({
                id: indSetting.yAxisName,
                title: {
                  text: "WILLIAM",
                },
                lineWidth: 1,
                min: -100,
                max: 0,
                top: this.ChartPosition.base + this.ChartPosition.gap,
                height: indSetting.height,
                plotLines: [
                  {
                    value: indSetting.threshold1,
                    color: this.ChartPosition.plotColor,
                    dashStyle: "shortdash",
                    width: 1,
                  },
                  {
                    value: indSetting.threshold2,
                    color: this.ChartPosition.plotColor,
                    dashStyle: "shortdash",
                    width: 1,
                  },
                ],
                labels: {
                  enabled: false,
                },

                opposite: true,
              });

              this.ChartPosition.base =
                this.ChartPosition.base +
                this.ChartPosition.gap +
                indSetting.height;
            }

            this.drawIndicator(indSetting.name, wrData, {
              yAxis: indSetting.yAxisName,
              color: indSetting.color,
            });
            break;
          default:
            this.drawIndicator(indSetting.name, values[i], indSetting);
        }
      }

      this.postDrawSetup();
    });
  };

  postDrawSetup = () => {
    // set date range
    if (
      this.context.state.selectedScreenPoint &&
      this.context.state.selectedScreenPoint
    ) {
      this.setScreenRange(this.context.state.selectedScreenPoint);
    }

    const newHeight = this.ChartPosition.base + this.ChartPosition.bottom;
    this.chart.setSize(null, newHeight);

    if (this.redrawLines) {
      this.removeLines();
      this.markScreenResult(this.props.screenResult);
    }
  };

  moveNextDay = () => {
    const foundIndex = this.tickers.findIndex(
      (ticker) => ticker[0] === this.chart.xAxis[0].max
    );
    try {
      var max =
        foundIndex + 1 >= this.tickers.length
          ? this.tickers[foundIndex][0]
          : this.tickers[foundIndex + 1][0];
      var min =
        foundIndex >= 99
          ? this.tickers[foundIndex - 99][0]
          : this.tickers[0][0];

      this.chart.xAxis[0].setExtremes(min, max);

      this.setTradeTicker(max);
    } catch (err) {
      console.log(err);
    }
  };

  setTradeTicker = (p_Stamp) => {
    const ticker = this.tickersOrigin.find((t) => t.p_Stamp === p_Stamp);
    this.context.setCurrentTradeTicker(ticker);
  };

  removeSeries = (name) => {
    const removeSeries = [];

    this.chart.series.forEach((s) => {
      if (s.name === name || s.name.indexOf(name) >= 0) {
        removeSeries.push(s.name);
      }
    });

    removeSeries.forEach((nameDel) => {
      this.chart.series.forEach((s) => {
        if (s.name === nameDel) {
          s.remove();
        }
      });
    });
  };

  drawIndicator = (name, data, settings = {}) => {
    var existingSeries = null;

    this.chart.series.forEach((series) => {
      if (series.name === name) {
        existingSeries = series;
      }
    });

    if (!existingSeries) {
      this.chart.addSeries(
        {
          name,
          type: settings.chartType ? settings.chartType : "line",
          data,
          lineWidth: 1,
          color: settings.color,
          yAxis: settings.yAxis ? settings.yAxis : 0,
        },
        true
      );
    } else {
      existingSeries.setData(data);
    }
  };

  markScreenResult = (screenResult) => {
    const ax = this.chart.xAxis[0];
    screenResult.forEach((match) => {
      ax.addPlotLine({
        value: match.p_Stamp,
        color: match.direction === "BUY" ? "blue" : "red",
        width: 1,
        id: match.p_Stamp,
      });
    });
  };

  removeLines = () => {
    const ax = this.chart.xAxis[0];
    const plotlines = [];
    ax.plotLinesAndBands.forEach((l) => {
      plotlines.push(l.id);
    });

    plotlines.forEach((id) => {
      ax.removePlotLine(id);
    });
  };

  testClicked = () => {};

  onTypeChange = (type) => {
    const setting = Object.assign(this.currentChartSettings);

    setting["type"] = type;
    this.context.updateChartSettings(this.chartName, setting);
  };

  onIndicatorChange = (ind) => {
    const setting = Object.assign(this.currentChartSettings);

    setting[ind] = !setting[ind];

    if (!setting[ind]) {
      this.removeSeries(ind);
      var indSetting = ChartHelper.getIndicatorSetting(ind);

      if (indSetting.ownPane) {
        const yAxisRemove = this.chart.get(indSetting.yAxisName);
        const topRemove = yAxisRemove.top;

        yAxisRemove.remove();

        var isLast = true;
        this.chart.yAxis.forEach((yx) => {
          if (yx.top > topRemove) {
            isLast = false;
            yx.update({
              top: yx.top - indSetting.height - this.ChartPosition.gap,
            });
          }
        });

        this.ChartPosition.base =
          this.ChartPosition.base - indSetting.height - this.ChartPosition.gap;
      }
    }
    this.prepareDrawChart();

    this.context.updateChartSettings(this.chartName, setting);
  };

  render() {
    return (
      <AppContext.Consumer>
        {(context) => {
          this.context = context;
          this.currentChartSettings = context.state[this.chartName]
            ? context.state[this.chartName]
            : this.defaultChartSetting;

          const state = this.currentChartSettings;
          return (
            <>
              <div className="row">
                <span>
                  <label className="radio-inline">
                    <input
                      type="radio"
                      name="type"
                      checked={state.type === "day"}
                      onChange={() => this.onTypeChange("day")}
                    />
                    Day
                  </label>
                  <label className="radio-inline">
                    <input
                      type="radio"
                      name="type"
                      checked={state.type === "week"}
                      onChange={() => this.onTypeChange("week")}
                    />
                    Week
                  </label>
                </span>
                <span style={{ paddingLeft: 20 }}>
                  <label className="checkbox-inline">
                    <input
                      type="checkbox"
                      value="ema10"
                      checked={state.ema10}
                      onChange={() => {
                        this.onIndicatorChange("ema10");
                      }}
                    />
                    ema10
                  </label>
                  <label className="checkbox-inline">
                    <input
                      type="checkbox"
                      value="ema20"
                      checked={state.ema20}
                      onChange={() => {
                        this.onIndicatorChange("ema20");
                      }}
                    />
                    ema20
                  </label>
                  <label className="checkbox-inline">
                    <input
                      type="checkbox"
                      value="ema50"
                      checked={state.ema50}
                      onChange={() => {
                        this.onIndicatorChange("ema50");
                      }}
                    />
                    ema50
                  </label>
                  <label className="checkbox-inline">
                    <input
                      type="checkbox"
                      value="sma100"
                      checked={state.sma100}
                      onChange={() => {
                        this.onIndicatorChange("sma100");
                      }}
                    />
                    sma100
                  </label>
                  <label className="checkbox-inline">
                    <input
                      type="checkbox"
                      value="sma200"
                      checked={state.sma200}
                      onChange={() => {
                        this.onIndicatorChange("sma200");
                      }}
                    />
                    sma200
                  </label>
                  <label className="checkbox-inline">
                    <input
                      type="checkbox"
                      value="bb"
                      checked={state.bb}
                      onChange={() => {
                        this.onIndicatorChange("bb");
                      }}
                    />
                    bb
                  </label>
                  <label className="checkbox-inline">
                    <input
                      type="checkbox"
                      value="macd"
                      checked={state.macd}
                      onChange={() => {
                        this.onIndicatorChange("macd");
                      }}
                    />
                    macd
                  </label>
                  <label className="checkbox-inline">
                    <input
                      type="checkbox"
                      value="adx"
                      checked={state.adx}
                      onChange={() => {
                        this.onIndicatorChange("adx");
                      }}
                    />
                    adx
                  </label>
                  <label className="checkbox-inline">
                    <input
                      type="checkbox"
                      value="heikin"
                      checked={state.heikin}
                      onChange={() => {
                        this.onIndicatorChange("heikin");
                      }}
                    />
                    heikin
                  </label>
                  <label className="checkbox-inline">
                    <input
                      type="checkbox"
                      value="stochastic"
                      checked={state.stochastic}
                      onChange={() => {
                        this.onIndicatorChange("stochastic");
                      }}
                    />
                    stoch
                  </label>
                  <label className="checkbox-inline">
                    <input
                      type="checkbox"
                      value="rsi"
                      checked={state.rsi}
                      onChange={() => {
                        this.onIndicatorChange("rsi");
                      }}
                    />
                    rsi
                  </label>
                  <label className="checkbox-inline">
                    <input
                      type="checkbox"
                      value="william"
                      checked={state.william}
                      onChange={() => {
                        this.onIndicatorChange("william");
                      }}
                    />
                    wr
                  </label>
                </span>
                <span>
                  <label className="checkbox-inline" style={{ marginLeft: 20 }}>
                    <button
                      type="button"
                      className="btn btn-primary btn-sm"
                      onClick={this.moveNextDay}
                    >
                      Next
                    </button>
                  </label>
                </span>
              </div>
              <div className="chart-wrapper">
                <div className="chart-inner">
                  <HighchartsReact
                    highcharts={Highcharts}
                    constructorType={"stockChart"}
                    options={this.state}
                    ref={this.chartRef}
                  />
                </div>
              </div>
            </>
          );
        }}
      </AppContext.Consumer>
    );
  }
}

export default StockChart;
