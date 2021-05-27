<template>
  <div>
    <page-head icon="chart-line" title="All Monitorings" />
    <div class="row">
      <div v-for="(item, index) in monitorings" :key="'monitoring-' + index">
        <div class="card mr-2 ml-2 mb-4 monitor-card">
          <div class="card-header pr-3 pl-3 d-flex">
            <div class="card-title mb-0 with-elements">
              <router-link
                :to="{
                  name: 'monitoring-view',
                  params: { id: item.monitorId }
                }"
              >
                <icon icon="chart-line" />
                {{ item.name }}
              </router-link>
              <div class="card-title-elements ml-md-auto">
                <monitor-status :status="item.stepStatus" :title="item.stepStatusText"/>
                <router-link class="d-none"
                  :to="{
                    name: 'monitoring-save',
                    params: { id: item.monitorId }
                  }"
                >
                  <icon icon="edit" />
                  Edit
                </router-link>
              </div>
            </div>
          </div>
          <apexchart
            type="area"
            :options="item.uptimeChart"
            :series="item.uptimeChart.series"
            class="m-2 mt-4"
            height="100"
          />
          <apexchart
            type="area"
            :options="item.loadtimeChart"
            :series="item.loadtimeChart.series"
            class="m-2 mt-4"
            height="100"
          />
          <div class="card-body">
            <div class="card-subtitle text-muted">
              <a
                :href="item.url"
                target="_blank"
                class="text-muted font-weight-light d-block text-ellipsis"
              >
                <icon icon="external-link-alt" />
                {{item.url}}
              </a>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import service from "service/monitoring";

export default {
  data() {
    return {
      monitorings: []
    };
  },
  async mounted() {
    var result = await service.list();
    if (result.data && result.data.length) {
      result.data.map(item => {
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
        this.monitorings.push(item);
      });
    }
  },
  methods: {
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
    },

  }
};
</script>
