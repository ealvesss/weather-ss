# weather-ss
Screen Scraping for forecast

#Url
- First parameter is country name (not upper or lower case sensitive)
- Second Parameter is city name (not upper or lower case sensitive) 
- http://localhost:49399/api/weather/**First Parameter**/**Second Parameter**

#Return
- All output data is in json format
- `{`
-     `\"Country\":\"Denmark\",`
-     `\"City\":\"Aarhus\",`
-     `\"Cond\":\"Mostly clear\",`
-     `\"Temp\":\"19°\",`
-     `\"dtTimeSearch\":\"8/28/16 23:35:17\"`
-   `}`

# Unit Tests
#### NoDataFound
      - Test when no data was found.
#### DataOk
      - Test if data is filled up.


#Observations
- I created a variable called **FakeDataBase** to hold all data about country, region and city. This variable is static to just hold all the data during the WebApi lifetime, so, after the first search the variable is filled with all data and this way dont need to hit the html parse again. It increase the search speed. 

- I used a paralelism to improve the html parse as well.  

