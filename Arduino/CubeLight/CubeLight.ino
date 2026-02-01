/*
 * Blink
 * Turns on the onboard LED on for one second, then off for one second, repeatedly.
 * This uses delay() to pause between LED toggles.
 */


#include <ESP8266WiFi.h>
#include <espnow.h>
#include <FastLED.h>

#define LED_PIN     2
#define NUM_LEDS    360
#define BRIGHTNESS  255
#define LED_TYPE    WS2812
#define COLOR_ORDER GRB
CRGB leds[NUM_LEDS];

#define UPDATES_PER_SECOND 100

// This example shows several ways to set up and use 'palettes' of colors
// with FastLED.
//
// These compact palettes provide an easy way to re-colorize your
// animation on the fly, quickly, easily, and with low overhead.
//
// USING palettes is MUCH simpler in practice than in theory, so first just
// run this sketch, and watch the pretty lights as you then read through
// the code.  Although this sketch has eight (or more) different color schemes,
// the entire sketch compiles down to about 6.5K on AVR.
//
// FastLED provides a few pre-configured color palettes, and makes it
// extremely easy to make up your own color schemes with palettes.
//
// Some notes on the more abstract 'theory and practice' of
// FastLED compact palettes are at the bottom of this file.



CRGBPalette16 currentPalette;
TBlendType    currentBlending;
CRGBPalette16 RedYellow;
CRGBPalette16 YellowRed;
uint8_t state = 0;
uint8_t Target = 155;
extern CRGBPalette16 myRedWhiteBluePalette;
extern const TProgmemPalette16 myRedWhiteBluePalette_p PROGMEM;

uint8_t brightness = 255;

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
        state = 0;
        //fill_solid( PaletteLQ, 16, CRGB::Black);
        Serial.println("received 0 signal");
        break;
      case 1:
        state = 1;
        Serial.println("received 1 signal");
        break;
      case 2:
        state = 2;
        //fill_solid( PaletteHQ, 16, CRGB::Black);
        Serial.println("received 2 signal");
        break;
      case 3:
        state = 3;
        Serial.println("received 3 signal");
        break;
      case 4:
        state = 4;
        Serial.println("received 4 signal");
        break;
      default:
        // Code to execute if none of the cases match
        Serial.println("received nothing");
        break;
      }


}



void setup() {
    Serial.begin(115200);
    delay( 3000 ); // power-up safety delay
    FastLED.addLeds<LED_TYPE, LED_PIN, COLOR_ORDER>(leds, NUM_LEDS).setCorrection( TypicalLEDStrip );
    FastLED.setBrightness(  BRIGHTNESS );
    
    // Color Options:  HeatColors_p LavaColors_p OceanColors_p PartyColors_p RainbowColors_p RainbowStripeColors_p CloudColors_p RainbowStripesColors_p ForestColors_p
    currentPalette = OceanColors_p;
    currentBlending = NOBLEND;
    
    state = 1;

    SetupRedAndYellowStripedPalette();
    SetupYellowAndRedStripedPalette();
}


void loop()
{
    //ChangePalettePeriodically();
    static uint8_t startIndex = 0;
    
    switch (state) {
      case 0:
        currentPalette = OceanColors_p;
        startIndex = startIndex + 1; /* motion speed */
        break;
      case 1:
        SwapColor();
        break;
      case 2:
        SwapColor();
        break;
      case 3:
        SwapColor();
        break;
      case 4:
        SwapColor();
        break;
      default:
        // Code to execute if none of the cases match
        Serial.println("received nothing");
        break;
      }
    
    FillLEDsFromPaletteColors( startIndex);
    
    FastLED.show();
    FastLED.delay(1000 / UPDATES_PER_SECOND);
}

void FillLEDsFromPaletteColors( uint8_t colorIndex)
{
    
    for( int i = 0; i < NUM_LEDS; i++) {
        leds[i] = ColorFromPalette( currentPalette, colorIndex, brightness, currentBlending);
        colorIndex += 3;
    }
}


// There are several different palettes of colors demonstrated here.
//
// FastLED provides several 'preset' palettes: RainbowColors_p, RainbowStripeColors_p,
// OceanColors_p, CloudColors_p, LavaColors_p, ForestColors_p, and PartyColors_p.
//
// Additionally, you can manually define your own color palettes, or you can write
// code that creates color palettes on the fly.  All are shown here.

void SwapColor()
{


    uint8_t secondHand = (millis() / 10) % 100;
    static uint8_t lastSecond = 99;
    
    if( lastSecond != secondHand) {
        if( secondHand < 50) currentPalette = RedYellow;
        else currentPalette = YellowRed;
    }
}





// This function fills the palette with totally random colors.
void SetupTotallyRandomPalette()
{
    for( int i = 0; i < 16; i++) {
        currentPalette[i] = CHSV( random8(), 255, random8());
    }
}

// This function sets up a palette of black and white stripes,
// using code.  Since the palette is effectively an array of
// sixteen CRGB colors, the various fill_* functions can be used
// to set them up.
void SetupRedAndYellowStripedPalette()
{
    // 'black out' all 16 palette entries...
    fill_solid( RedYellow, 16, CRGB::Yellow);
    // and set every fourth one to white.
    RedYellow[0] = CRGB::Red;
    RedYellow[2] = CRGB::Red;
    RedYellow[4] = CRGB::Red;
    RedYellow[6] = CRGB::Red;
    RedYellow[8] = CRGB::Red;
    RedYellow[10] = CRGB::Red;
    RedYellow[12] = CRGB::Red;
    RedYellow[14] = CRGB::Red;
    
}

void SetupYellowAndRedStripedPalette()
{
    // 'black out' all 16 palette entries...
    fill_solid( YellowRed, 16, CRGB::Red);
    // and set every fourth one to white.
    YellowRed[0] = CRGB::Yellow;
    YellowRed[2] = CRGB::Yellow;
    YellowRed[4] = CRGB::Yellow;
    YellowRed[6] = CRGB::Yellow;
    YellowRed[8] = CRGB::Yellow;
    YellowRed[10] = CRGB::Yellow;
    YellowRed[12] = CRGB::Yellow;
    YellowRed[14] = CRGB::Yellow;
}

// This function sets up a palette of purple and green stripes.
void SetupPurpleAndGreenPalette()
{
    CRGB purple = CHSV( HUE_PURPLE, 255, 255);
    CRGB green  = CHSV( HUE_GREEN, 255, 255);
    CRGB black  = CRGB::Black;
    
    currentPalette = CRGBPalette16(
                                   green,  green,  black,  black,
                                   purple, purple, black,  black,
                                   green,  green,  black,  black,
                                   purple, purple, black,  black );
}


// This example shows how to set up a static color palette
// which is stored in PROGMEM (flash), which is almost always more
// plentiful than RAM.  A static PROGMEM palette like this
// takes up 64 bytes of flash.
const TProgmemPalette16 myRedWhiteBluePalette_p PROGMEM =
{
    CRGB::Red,
    CRGB::Gray, // 'white' is too bright compared to red and blue
    CRGB::Blue,
    CRGB::Black,
    
    CRGB::Red,
    CRGB::Gray,
    CRGB::Blue,
    CRGB::Black,
    
    CRGB::Red,
    CRGB::Red,
    CRGB::Gray,
    CRGB::Gray,
    CRGB::Blue,
    CRGB::Blue,
    CRGB::Black,
    CRGB::Black
};



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
