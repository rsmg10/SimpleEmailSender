# EmailSender
add the following options to the appsetting.json


  "EmailSenderOptions": {
    "EmailDomain": "smtp.gmail.com",
    "Email": "", // smtp email
    "AppPassword": "", // generated App password from // https://myaccount.google.com/apppasswords 
    "Port": 465, // 587 
    "AllowedContentType": "application/pdf, application/json" // allowed attachment types
  },
  
  alternativery, if the config not found, you can use environment varialbls to set them 
  by pasting the following lines in the command prompt
  
set Email=<userEmail>
set EmailDomain=smtp.gmail.com
set AppPassword=<your app password>
set Port=<portNumber>
set AllowedContentType=application/pdf
  
  when setting enviroment varialbe, ensure that there is no spaces aroung the '=' signs, and that the values are not wrapped in quotation marks. 
  
  if you opt to use the enviroment varialbe option, you must run the project using   'dotnet run' using the command prompt, otherwise the project would not find the variables
  
  
  
  add the follwoing line in the program.cs file 
    builder.Services.ConfigureEmailSender();
  this line will inject the Services adn interface and configure the options
  
  now you can use the IEmailSender interface with method SendEmail
