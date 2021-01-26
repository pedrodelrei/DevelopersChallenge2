<template>
  <div id="app">
    <img alt="Vue logo" src="./assets/logo.png">
  </div>
</template>

<script>
import axios from'axios'

export default {
  name: 'App',
  data() {
    return {
      apiPath: "https://localhost:5001/api/values",
    }
  },
  mounted() {
    this.accessAPI();
  },
  methods: {
    accessAPI(){
      axios.get(this.apiPath)
        .then(response => {
          this.posts=response.data;
        })
        .catch(error => {
          console.log('-----error-------');
          console.log(error);
        })
    }
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
