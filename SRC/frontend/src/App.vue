<template>
  <v-app>
    <v-app-bar
      app
      color="primary"
      dark
    >
      <div class="d-flex align-center">
        <v-btn-toggle
          mandatory
        >
        <v-btn
          color="primary"
          @click="changeView('trans')">
          <span >View Transactions</span>
        </v-btn>
        
      <v-spacer></v-spacer>
        <v-btn
          color="primary"
          @click="changeView('upload')">
          <span >Upload New OFX File</span>
        </v-btn>
        
      </v-btn-toggle>
      </div>
    </v-app-bar>

    <v-main>
      <TransactionList v-if="ShowTransactions"/>
      <FileUpload v-if="!ShowTransactions"/>
    </v-main>
  </v-app>
</template>

<script>
import TransactionList from './components/TransactionList';
import FileUpload from './components/FileUpload';

export default {
  name: 'App',

  components: {
    TransactionList,
    FileUpload
  },

  data: () => ({
    ShowTransactions: true
  }),
  methods: {
    changeView(view) {
      this.ShowTransactions = view == "trans";
    }
  }
};
</script>
