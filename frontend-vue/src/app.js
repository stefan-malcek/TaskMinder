import express from 'express'

const app = express()

app.use(express.static('dist'))


// const port = env.PORT ?? 8080;
const port =  8080;

app.listen(port, () => {
  console.log('Server listening on: http://localhost:%s', port)
})