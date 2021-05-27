import { http } from "utils/http";
const MonitoringService = {
  async list() {
    var result = await http.get("/api/monitoring");
    if (result.status === 200) {
      return result.data;
    } else {
      console.error(result.error);
      throw result.error;
    }
  },
  async get(id) {
    var result = await http.get("/api/monitoring/" + id);
    if (result.status === 200) {
      return result.data;
    } else {
      console.error(result.error);
      throw result.error;
    }
  },
  async steps(id) {
    var result = await http.get("/api/monitoring/steps/" + id);
    if (result.status === 200) {
      return result.data;
    } else {
      console.error(result.error);
      throw result.error;
    }
  },
  async steplogs(id, page) {
    if(!page){
      page = 0;
    }
    var result = await http.get(`/api/monitoring/steplogs/${id}?page=${page}`);
    if (result.status === 200) {
      return result.data;
    } else {
      console.error(result.error);
      throw result.error;
    }
  },
  async save(value) {
    var result = await http.post("/api/monitoring", value);
    if (result.status === 200) {
      return result.data;
    } else {
      console.error(result.error);
      throw result.error;
    }
  }
}
export default MonitoringService;
