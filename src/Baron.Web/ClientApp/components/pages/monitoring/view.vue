<template>
  <div>
    <div v-if="!item">
      <content-placeholders>
        <content-placeholders-text :lines="3" />
      </content-placeholders>
    </div>
    <div v-if="item">
      <page-head title="Dashboard" prefix="cagricomen" icon="chart-line" />
      <div class="row">
        <div class="col-sm-6 col-xl-6">
          <div class="card mb-4">
             <apexchart
                type="area"
                :options="item.uptimeChart"
                :series="item.uptimeChart.series"
                class="m-2 mt-4"
                height="100"
              />
            <!----><!---->
          </div>
        </div>
        <div class="col-sm-6 col-xl-6">
          <div class="card mb-4">
            <apexchart
              type="area"
              :options="item.loadtimeChart"
              :series="item.loadtimeChart.series"
              class="m-2 mt-4"
              height="100"
            />
            <!----><!---->
          </div>
        </div>
      </div>
      <div class="row">
        <div class="col-md-12">
          <div class="card mb-4">
            <h6 class="card-header with-elements">
              <div class="card-header-title">Sale stats</div>
              <div class="card-header-elements ml-auto d-none">
                <button
                  type="button"
                  class="btn btn-default btn-xs md-btn-flat"
                >
                  Show more
                </button>
              </div>
            </h6>
            <div class="table-responsive">
              <table class="table card-table" v-if="steps">
                <thead>
                  <tr>
                    <th>...</th>
                    <th>Type</th>
                    <th>Last Check Date</th>
                    <th>Status</th>
                    <th>Interval</th>
                  </tr>
                </thead>
                <tbody>
                  <tr
                    v-for="(item, index) in steps"
                    :key="'monitorstep' + index"
                  >
                    <td>
                      <button
                        v-b-modal.modal-lg
                        @click="details(item.monitorStepId)"
                        class="btn btn-primary"
                      >
                        <icon icon="search" />
                      </button>
                    </td>
                    <td>{{ item.typeText }}</td>
                    <td>
                      <span :title="item.lastCheckDate">{{
                        item.lastCheckDate | moment("from", "now")
                      }}</span>
                    </td>
                    <td>
                      <monitor-status
                        :status="item.status"
                        :title="item.statusText"
                      />
                    </td>
                    <td>{{ item.interval }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>
    <b-modal id="modal-lg" size="lg" title="Logs">
      <div class="table-responsive">
        <table class="table card-table" v-if="logs">
          <thead>
            <tr>
              <th>Start</th>
              <th>End</th>
              <th>Status</th>
              <th>Interval</th>
              <th>Log</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="(item, index) in logs.items"
              :key="'monitorsteplog' + index"
            >
              <td>
                <span :title="item.startDate">{{
                  item.startDate | moment()
                }}</span>
              </td>
              <td>
                <span :title="item.endDate">{{ item.endDate | moment() }}</span>
              </td>
              <td>
                <monitor-status
                  :status="item.status"
                  :title="item.statusText"
                />
              </td>
              <td>{{ item.interval }}</td>
              <td>{{ item.log }}</td>
            </tr>
          </tbody>
        </table>
        <b-pagination
          v-if="logs"
          :total-rows="logs.itemCount"
          v-model="logsCurrentPage"
          :per-page="10"
          size="lg"
        ></b-pagination>
      </div>
    </b-modal>
  </div>
</template>

<script>
import service from "service/monitoring";

export default {
  data() {
    return {
      id: null,
      item: null,
      steps: null,
      logs: null,
      logsCurrentPage: 1,
      currentStepId: null
    };
  },
  watch: {
    async logsCurrentPage(newValue) {
      this.logPageChanged(newValue);
    }
  },
  async mounted() {
    const result = await service.get(this.$route.params.id);
    if (result.success) {
      let item = result.data;
      item.loadtimeChart = this.chart(
        `${item.loadTime.toFixed(2)} ms`,
        "Load Time",
        item.loadTimes
      );
      item.uptimeChart = this.chart(
        `${item.upTime.toFixed(2)} %`,
        "Up Time",
        item.upTimes
      );
      this.item = item;
    }

    const steps = await service.steps(this.$route.params.id);
    console.log(steps);
    this.steps = steps.data;
  },
  methods: {
    async details(id) {
      this.currentStepId = id;
      await this.logPageChanged(1);
    },
    async logPageChanged(page) {
      this.logs = null;
      const result = await service.steplogs(this.currentStepId, page - 1);
      this.logs = result.data;
    },
    chart(title, subtitle, data) {
      return {
        chart: {
          type: "area",
          height: 160,
          sparkline: {
            enabled: true
          }
        },
        stroke: {
          curve: "straight"
        },
        fill: {
          opacity: 0.3
        },
        series: [
          {
            name: subtitle,
            data: data
          }
        ],
        yaxis: {
          min: 0
        },
        colors: ["#00bfff"],
        title: {
          text: title,
          style: {
            fontSize: "16pt",
            cssClass: "apexcharts-yaxis-title"
          }
        },
        subtitle: {
          text: subtitle,
          offsetX: 0,
          style: {
            fontSize: "10px",
            cssClass: "apexcharts-yaxis-title"
          }
        }
      };
    }
  }
};
</script>
