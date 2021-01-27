<template>
  <v-container>
    <v-row class="text-center">
      <v-col class="mb-4">
          <v-data-table
            :loading="loadingData"
            item-key="id"
            sort-by="date"
            :headers="headers"
            :items="transactions"
            :items-per-page="10"
            class="elevation-1"
          >
          <template v-slot:[`item.value`]="{ item }">
            <v-chip
              :color="getColor(item.value)"
              dark
            >
                {{ item.value }}
              </v-chip>
          </template>
          </v-data-table>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
  import axios from "axios"
  export default {
    name: 'FileUpload',

    data: () => ({
      loadingData: false,
      accountIds: [],

      headers: [
          {
            text: 'Account',
            align: 'start',
            sortable: false,
            value: 'accountId',
          },
          {
            text: 'Date (dd/mm/yyyy)',
            sortable: false,
            value: 'date',
          },
          {
            text: 'Type',
            sortable: false,
            value: 'transType',
          },
          {
            text: 'Value',
            sortable: false,
            value: 'value',
          },
          {
            text: 'Memo',
            sortable: false,
            value: 'memo',
          },
      ],
      transactions: [
      ]
    }),
    methods:{
      loadTransactions() {
        var self = this;
        this.loadingData = true;
        axios.get('https://localhost:5001/api/transactions'
        ).then(function(response){
          self.transactions = response.data;
          self.loadingData = false;
        })
        .catch(function(){
          self.loadingData = false;
        });
      },
      getColor (value) {
        if (value[0] == '-') return 'red'
        else return 'green'
      },
    },
    mounted(){
        this.loadTransactions();
    },
  }
</script>
