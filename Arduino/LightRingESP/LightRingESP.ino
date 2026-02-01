/*
 * Blink
 * Turns on the onboard LED on for one second, then off for one second, repeatedly.
 * This uses delay() to pause between LED toggles.
 */


#include <ESP8266WiFi.h>
#include <espnow.h>
#include <FastLED.h>

#define LED_PIN     2
#define NUM_LEDS    135
#define NUM_SIDE    30
#define NUM_LQ      45
#define NUM_HQ      90

#define BRIGHTNESS  100
#define LED_TYPE    WS2812
#define COLOR_ORDER GRB
CRGB leds[NUM_LEDS];

#define UPDATES_PER_SECOND 100

bool ledOn=false;

#define STATUS_LED BUILTIN_LED

CRGBPalette16 currentPalette;
CRGBPalette16 PaletteHQ;
CRGBPalette16 PaletteLQ;
TBlendType    currentBlending;
TBlendType    BlendingHQ;
TBlendType    BlendingLQ;

extern CRGBPalette16 myRedWhiteBluePalette;
extern const TProgmemPalette16 myRedWhiteBluePalette_p PROGMEM;

bool ledOnA;
bool ledOnB;

// Set your new MAC Address
//uint8_t newMACAddress[] = {0x05, 0xCC, 0xDF, 0x36, 0x97, 0x71};

// Structure example to receive data
// Must match the sender structure
typedef struct test_struct {
    int x;
} test_struct;

// Create a struct_message called myData
test_struct myData;

// Callback function that will be executed when data is received
void OnDataRecv(uint8_t * mac, uint8_t *incomingData, uint8_t len) {
  memcpy(&myData, incomingData, sizeof(myData));



    switch (myData.x) {
      case 0:
        ledOnA = false; // Start with the light off  
        ledOn=false;
        fill_solid( PaletteLQ, 16, CRGB::Black);
        Serial.println("received 0 signal");
        break;
      case 1:
        ledOnA = true;
        ledOn=true;
        SetupBlackAndWhiteStripedPalette(); 
        Serial.println("received 1 signal");
        break;
      case 2:
        ledOnB = false;
        fill_solid( PaletteHQ, 16, CRGB::Black);
        Serial.println("received 2 signal");
        break;
      case 3:
        ledOnB = true;
        PaletteHQ = RainbowStripeColors_p;
        Serial.println("received 3 signal");
        break;
      default:
        // Code to execute if none of the cases match
        Serial.println("received nothing");
        break;
      }
    digitalWrite(BUILTIN_LED, ledOn);
    //PaletteLQ = CloudColors_p;


}


void setup() {
    Serial.begin(115200);
    delay( 3000 ); // power-up safety delay

    pinMode(BUILTIN_LED, OUTPUT);  // initialize onboard LED as output
    digitalWrite(BUILTIN_LED, 1); // Start with the light off  

    FastLED.addLeds<LED_TYPE, LED_PIN, COLOR_ORDER>(leds, NUM_LEDS).setCorrection( TypicalLEDStrip );
    FastLED.setBrightness(  BRIGHTNESS );
    
    // Color Options:  HeatColors_p LavaColors_p OceanColors_p PartyColors_p RainbowColors_p RainbowStripeColors_p CloudColors_p RainbowStripesColors_p ForestColors_p
    PaletteHQ = RainbowStripeColors_p;
    //PaletteLQ = CloudColors_p;
    SetupBlackAndWhiteStripedPalette();     
    BlendingHQ = NOBLEND;
    BlendingLQ = NOBLEND;
    Serial.println("Startup");
    ledOnA = true;
    ledOnB = true;

     WiFi.mode(WIFI_STA);
     //wifi_set_macaddr(STATION_IF, &newMACAddress[0]);
    //Serial.print("[NEW] ESP8266 Board MAC Address:  ");
   // Serial.println(WiFi.macAddress());
    WiFi.disconnect();

  // Init ESP-NOW
  if (esp_now_init() != 0) {
    Serial.println("Error initializing ESP-NOW");
    return;
  }
  
  // Once ESPNow is successfully Init, we will register for recv CB to
  // get recv packer info
  esp_now_set_self_role(ESP_NOW_ROLE_SLAVE);
  esp_now_register_recv_cb(OnDataRecv);

}


void loop()
{
    //ChangePalettePeriodically();
    
    
    static uint8_t startIndex = 0;
    startIndex = startIndex + 1; /* motion speed */
    //Serial.println(startIndex);
    FillLEDsFromPaletteColors( startIndex);
    
    
    FastLED.show();
    FastLED.delay(1000 / UPDATES_PER_SECOND);
}

void FillLEDsFromPaletteColors( uint8_t colorIndex)
{
    uint8_t brightness = 255;
    
    

    for( int i = 0; i < NUM_LEDS; i++) {
        
        if (i<NUM_LQ){
            leds[i] = ColorFromPalette( PaletteLQ, colorIndex, brightness, BlendingLQ);
            }
        else {
            leds[i] = ColorFromPalette( PaletteHQ, colorIndex, brightness, BlendingHQ);
            }
            
        colorIndex += 3;
    }
}



// This function sets up a palette of black and white stripes,
// using code.  Since the palette is effectively an array of
// sixteen CRGB colors, the various fill_* functions can be used
// to set them up.
void SetupBlackAndWhiteStripedPalette()
{
    // 'black out' all 16 palette entries...
    fill_solid( PaletteLQ, 16, CRGB::Black);
    // and set every fourth one to white.
    PaletteLQ[0] = CRGB::White;
    PaletteLQ[2] = CRGB::White;
    PaletteLQ[4] = CRGB::White;
    PaletteLQ[6] = CRGB::White;
    PaletteLQ[8] = CRGB::White;
    PaletteLQ[10] = CRGB::White;
    PaletteLQ[12] = CRGB::White;
    PaletteLQ[14] = CRGB::White;
    
}



// Additionl notes on FastLED compact palettes:
//
// Normally, in computer graphics, the palette (or "color lookup table")
// has 256 entries, each containing a specific 24-bit RGB color.  You can then
// index into the color palette using a simple 8-bit (one byte) value.
// A 256-entry color palette takes up 768 bytes of RAM, which on Arduino
// is quite possibly "too many" bytes.
//
// FastLED does offer traditional 256-element palettes, for setups that
// can afford the 768-byte cost in RAM.
//
// However, FastLED also offers a compact alternative.  FastLED offers
// palettes that store 16 distinct entries, but can be accessed AS IF
// they actually have 256 entries; this is accomplished by interpolating
// between the 16 explicit entries to create fifteen intermediate palette
// entries between each pair.
//
// So for example, if you set the first two explicit entries of a compact 
// palette to Green (0,255,0) and Blue (0,0,255), and then retrieved 
// the first sixteen entries from the virtual palette (of 256), you'd get
// Green, followed by a smooth gradient from green-to-blue, and then Blue.
