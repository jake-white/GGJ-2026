/*
  Rui Santos
  Complete project details at https://RandomNerdTutorials.com/esp-now-one-to-many-esp8266-nodemcu/
  
  Permission is hereby granted, free of charge, to any person obtaining a copy
  of this software and associated documentation files.
  
  The above copyright notice and this permission notice shall be included in all
  copies or substantial portions of the Software.
*/

#include <ESP8266WiFi.h>
#include <espnow.h>

bool ledOn=false;
bool buttonDown=false;

#define STATUS_LED BUILTIN_LED

// REPLACE WITH RECEIVER MAC Address
uint8_t broadcastAddress1[] = {0xD8, 0xBF, 0xC0, 0xC2, 0x36, 0x49};
uint8_t broadcastAddress2[] = {0xF4, 0xCF, 0xA2, 0xFE, 0x6B, 0xD1};
uint8_t broadcastAddress3[] = {0x3C, 0xE9, 0x0E, 0xCB, 0x95, 0xBF};
uint8_t broadcastAddress4[] = {0x84, 0xF3, 0xEB, 0x42, 0xAA, 0x8D};
uint8_t broadcastAddress5[] = {0x84, 0x0D, 0x8E, 0x87, 0x89, 0x20};
uint8_t broadcastAddress6[] = {0xCC, 0x50, 0xE3, 0x65, 0x75, 0x6D};
uint8_t broadcastAddress7[] = {0xA0, 0x20, 0xA6, 0x1F, 0x58, 0xBC};

// Structure example to send data
// Must match the receiver structure
typedef struct test_struct {
    int x;
} test_struct;

// Create a struct_message called test to store variables to be sent
test_struct test;

// Callback when data is sent
void OnDataSent(uint8_t *mac_addr, uint8_t sendStatus) {
  char macStr[18];
  Serial.print("Packet to:");
  snprintf(macStr, sizeof(macStr), "%02x:%02x:%02x:%02x:%02x:%02x",
         mac_addr[0], mac_addr[1], mac_addr[2], mac_addr[3], mac_addr[4], mac_addr[5]);
  Serial.print(macStr);
  Serial.print(" send status: ");
  if (sendStatus == 0){
    Serial.println("Delivery success");
  }
  else{
    Serial.println("Delivery fail");
  }
}

void setup() {
  // Init Serial Monitor
  Serial.begin(115200);

  pinMode(BUILTIN_LED, OUTPUT);  // initialize onboard LED as output
  digitalWrite(BUILTIN_LED, 1); // Start with the light off  
  
  // Set device as a Wi-Fi Station
  WiFi.mode(WIFI_STA);
  WiFi.disconnect();

  // Init ESP-NOW
  if (esp_now_init() != 0) {
    Serial.println("Error initializing ESP-NOW");
    return;
  }

  esp_now_set_self_role(ESP_NOW_ROLE_CONTROLLER);
  
  // Once ESPNow is successfully Init, we will register for Send CB to
  // get the status of Trasnmitted packet
  esp_now_register_send_cb(OnDataSent);
  
  // Register peer
  esp_now_add_peer(broadcastAddress1, ESP_NOW_ROLE_SLAVE, 1, NULL, 0);
  esp_now_add_peer(broadcastAddress2, ESP_NOW_ROLE_SLAVE, 1, NULL, 0);
  esp_now_add_peer(broadcastAddress3, ESP_NOW_ROLE_SLAVE, 1, NULL, 0);
  esp_now_add_peer(broadcastAddress4, ESP_NOW_ROLE_SLAVE, 1, NULL, 0);
  esp_now_add_peer(broadcastAddress5, ESP_NOW_ROLE_SLAVE, 1, NULL, 0);
  esp_now_add_peer(broadcastAddress6, ESP_NOW_ROLE_SLAVE, 1, NULL, 0);
  esp_now_add_peer(broadcastAddress7, ESP_NOW_ROLE_SLAVE, 1, NULL, 0);

 
}


 
void loop() {
  if (Serial.available() > 0) {
    //String receivedString = Serial.readString();
    char receivedChar = Serial.read(); // Read a single character
    
    // Convert the string to an integer
    //int boardNum = receivedString.charAt(0) - '0';
    //int boardStatus = receivedString.charAt(1) - '0';
  
    switch (receivedChar) {
      
      case 'a':
        digitalWrite(BUILTIN_LED, 0); //Built in LED On
        break;
	    case 'b':
        digitalWrite(BUILTIN_LED, 1); //Built in LED OFF  
        break;
	    case 'c':
        test.x = 3;
        esp_now_send(broadcastAddress1, (uint8_t *) &test, sizeof(test));
        break;
	    case 'd':
        test.x = 2;
        esp_now_send(broadcastAddress1, (uint8_t *) &test, sizeof(test));
        break;
	    case 'e':
        test.x = 1;
        esp_now_send(broadcastAddress1, (uint8_t *) &test, sizeof(test));
        break;
	    case 'f':
        test.x = 0;
        esp_now_send(broadcastAddress1, (uint8_t *) &test, sizeof(test));
        break;
	    case 'g':
        test.x = 3;
        esp_now_send(broadcastAddress2, (uint8_t *) &test, sizeof(test));
        break;
	    case 'h':
        test.x = 2;
        esp_now_send(broadcastAddress2, (uint8_t *) &test, sizeof(test));
        break;
	    case 'i':
        test.x = 1;
        esp_now_send(broadcastAddress2, (uint8_t *) &test, sizeof(test));
        break;
	    case 'j':
        test.x = 0;
        esp_now_send(broadcastAddress2, (uint8_t *) &test, sizeof(test));
        break;
	    case 'k':
        test.x = 3;
        esp_now_send(broadcastAddress3, (uint8_t *) &test, sizeof(test));
        break;
	    case 'l':
        test.x = 2;
        esp_now_send(broadcastAddress3, (uint8_t *) &test, sizeof(test));
        break;
	    case 'm':
        test.x = 1;
        esp_now_send(broadcastAddress3, (uint8_t *) &test, sizeof(test));
        break;
	    case 'n':
        test.x = 0;
        esp_now_send(broadcastAddress3, (uint8_t *) &test, sizeof(test));
        break;
	    case 'o':
        test.x = 3;
        esp_now_send(broadcastAddress4, (uint8_t *) &test, sizeof(test));
        break;
	    case 'p':
        test.x = 2;
        esp_now_send(broadcastAddress4, (uint8_t *) &test, sizeof(test));
        break;
	    case 'q':
        test.x = 1;
        esp_now_send(broadcastAddress4, (uint8_t *) &test, sizeof(test));
        break;
	    case 'r':
        test.x = 0;
        esp_now_send(broadcastAddress4, (uint8_t *) &test, sizeof(test));
        break;
	    case 's':
        test.x = 3;
        esp_now_send(broadcastAddress5, (uint8_t *) &test, sizeof(test));
        break;
	    case 't':
        test.x = 2;
        esp_now_send(broadcastAddress5, (uint8_t *) &test, sizeof(test));
        break;
	    case 'u':
        test.x = 1;
        esp_now_send(broadcastAddress5, (uint8_t *) &test, sizeof(test));
        break;
	    case 'v':
        test.x = 0;
        esp_now_send(broadcastAddress5, (uint8_t *) &test, sizeof(test));
        break;
	    case 'w':
        test.x = 3;
        esp_now_send(broadcastAddress6, (uint8_t *) &test, sizeof(test));
        break;
	    case 'x':
        test.x = 2;
        esp_now_send(broadcastAddress6, (uint8_t *) &test, sizeof(test));
        break;
	    case 'y':
        test.x = 1;
        esp_now_send(broadcastAddress6, (uint8_t *) &test, sizeof(test));
        break;
	    case 'z':
        test.x = 0;
        esp_now_send(broadcastAddress6, (uint8_t *) &test, sizeof(test));
        break;
      case '0':
        test.x = 0;
        esp_now_send(broadcastAddress7, (uint8_t *) &test, sizeof(test)); 
        break;
      case '1':
        test.x = 1;
        esp_now_send(broadcastAddress7, (uint8_t *) &test, sizeof(test)); 
        break;
      case '2':
        test.x = 2;
        esp_now_send(broadcastAddress7, (uint8_t *) &test, sizeof(test));
        break;
      case '3':
        test.x = 3;
        esp_now_send(broadcastAddress7, (uint8_t *) &test, sizeof(test));
        break;
      case '4':
        test.x = 4;
        esp_now_send(broadcastAddress7, (uint8_t *) &test, sizeof(test));
        break;
      case '5':
        test.x = 5;
        esp_now_send(broadcastAddress7, (uint8_t *) &test, sizeof(test));
        break;
      default:
        // Code to execute if none of the cases match
        break;
      }
  }
  
   /* if (Serial.available() > 0) {
      char receivedChar = Serial.read(); // Read a single character
  
      if (receivedChar == '1') {
        triggerFunctionOne(); // Call your function
      } else if (receivedChar == '0') {
        triggerFunctionTwo(); // Call another function
      }
    }
    
    if (!digitalRead(STATUS_BUTTON)){
      Serial.println("Button Pressed!");
      if (!buttonDown){
        buttonDown = true;
        ledOn = !ledOn;

        if (ledOn){
          test.x = 1;
          esp_now_send(0, (uint8_t *) &test, sizeof(test));
          }
         else{
          test.x = 0;
          esp_now_send(0, (uint8_t *) &test, sizeof(test));
          }
        }
        delay(300);
      }
      else{
        buttonDown = false;
        }
    */
  }
/*
void triggerFunction(int board, int msg) {
  // Code for the first function (e.g., turn on an LED)
  test.x = msg;
  
  esp_now_send(0, (uint8_t *) &test, sizeof(test));

}
*/
