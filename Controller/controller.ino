#include <Keyboard.h>

const int joystickClick = 2;
const int joystickX = A1;
const int joystickY = A0;

const int jumpPin = 8;
const int dashPin = 7;

void setup() {
  Serial.begin( 9600 );
  pinMode(jumpPin, INPUT_PULLUP);
  pinMode(dashPin, INPUT_PULLUP);

  Keyboard.begin();
}

void readJoystick() {
  Serial.print(analogRead(joystickX), DEC);
  Serial.print(",");
  Serial.print(analogRead(joystickY), DEC);
  Serial.print(",");
  Serial.print(digitalRead(joystickClick));
  Serial.print("\n");
  delay(15);
}

void readClicks() {
  if(digitalRead(jumpPin) == LOW) {
    Keyboard.press('w');
  } else if(digitalRead(dashPin) == LOW) {
    Keyboard.press(KEY_LEFT_SHIFT);
  } else {
    Keyboard.releaseAll();
  }
}

void loop() {
  readJoystick();
  readClicks();
}
