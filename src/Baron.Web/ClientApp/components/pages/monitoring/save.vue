<template>
  <div>
    <page-head icon="plus" title="New Monitoring" />

    <bi-text title="Project Name" placeholder="Type your project name" v-model="model.name" />
    <bi-text title="Project Url" placeholder="Type your website url for test" v-model="model.url" />
    <button @click="save" class="btn btn-primary">
      Save
    </button>
  </div>
</template>
<script>
import service from "service/monitoring";
import router from '@/router'
export default {
  data() {
    return {
      model: {
        name: "",
        url: ""
      }
    };
  },
  async mounted() {
    if(this.$route.params.id){
      let result = await service.get(this.$route.params.id);
      if(result.success){
        this.model.name = result.data.name;
        this.model.url = result.data.url;
      }
    }
  },
  methods: {
    async save() {
       if(this.$route.params.id) {
         this.model.id = this.$route.params.id;
       }
      let result = await service.save(this.model);
      if(result.success && result.data && result.data.id){
        router.push({
          name: "monitoring-list"
        });
      }
    }
  }
};
</script>

