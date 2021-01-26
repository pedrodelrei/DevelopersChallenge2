<template>
  <v-container>
    <v-row class="text-center">
      <v-col class="mb-4">
        <h1 class="display-2 font-weight-bold mb-3">
          Upload a File
        </h1>
        <p class="subheading font-weight-regular">
          To upload a file, simply click on <b>Import OFX File</b>
          <br>
          button and then click on the button to it's right.
        </p>
      </v-col>

      <v-col
        class="mb-0 pb-0"
        cols="12"
      >
      <v-btn
        color="primary"
        class="text-none"
        rounded
        depressed
        :loading="isSelecting"
        @click="onButtonClick"
      >
        <v-icon left>
         mdi-cloud-upload
        </v-icon>
        {{ buttonText }}
      </v-btn>
      <input
        ref="uploader"
        class="d-none"
        type="file"
        accept=".ofx"
        @change="onFileChanged"
      >
      <v-tooltip bottom>
      <template v-slot:activator="{ on, attrs }">
      <v-btn
        :disabled="!selectedFile"
        color="blue-grey"
        class="ma-2 white--text"
        fab
        v-bind="attrs"
        v-on="on"
        v-on:click="submitFile()"
      >
      <v-icon dark>
        mdi-cloud-upload
      </v-icon>
    </v-btn>
      </template>
      <span>Upload File</span>
    </v-tooltip>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
  import axios from "axios"
  export default {
    name: 'FileUpload',

    data: () => ({
      selectedFile: null,
      isSelecting: false,
      defaultButtonText: 'Import OFX File',
    }),
  computed: {
    buttonText() {
      return this.selectedFile ? this.selectedFile.name : this.defaultButtonText
    }
  },
  methods: {
    onButtonClick() {
      this.isSelecting = true
      window.addEventListener('focus', () => {
        this.isSelecting = false;
      }, { once: true })

      this.$refs.uploader.click()
    },
    submitFile() {
      let formData = new FormData();
      formData.append('file', this.selectedFile);
      axios.post( 'https://localhost:5001/api/values',
        formData,
        {
          headers: {
            'Content-Type': 'multipart/form-data'
          }
        }
      ).then(function(){
        console.log('SUCCESS!!');
      })
      .catch(function(){
        console.log('FAILURE!!');
      });
    },
    onFileChanged(e) {
      this.selectedFile = e.target.files[0]
    }
    
  }
  }
</script>
