const express = require('express');
const path = require('path');
const bodyParser = require('body-parser');
//const nodeMailer = require('nodemailer');
const home = require('./routes/home');
const events = require('./routes/events');
const filters = require('./routes/filters');
const mustache = require('mustache');
const fs = require("fs");

var app = express();

app.set('view engine','html');
// To set functioning of mustachejs view engine
app.engine('html', function (filePath, options, callback) { 
    fs.readFile(filePath, function (err, content) {
        if(err)
            return callback(err)        
        var rendered = mustache.to_html(content.toString(),options);
        return callback(null, rendered)
    });
});

// Setting mustachejs as view engine
app.set('views', path.join(__dirname,'views'));

//Define routes:
app.use('/', home);
app.use('/events/', events);
app.use('/filters/', filters);
app.use(express.static(path.join(__dirname,'/static')));


app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
    extended:false
}));

const port = process.env.PORT || 3000;
app.listen(port, ()=>{
    console.log(`Listening on port ${port}`);
});