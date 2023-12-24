\ main.fs
\  Thu  6 Jan 17:55:54 UTC 2022

target
turnkey
    decimal
    initGPIO


: test  cr cr ." this is going to be good " cr
  ." meta-compilation in gforth " cr
  ." code reduction - reduced memory." cr cr
  ." Sun 24 Dec 17:50:02 UTC 2023";

: tsecxpj tusec ; \ wait 10 usec   - 10 microseconds
: tsecrrr msec ;  \ wait 1000 usec - 1 millisecond

: timed 10000 #, for tusec next ;

: blinque led on timed  led off timed timed timed ;

: demc $1B #, for blinque next ;

: demob stop 8000 #, ms
       start 2000 #, ms
        stop 8000 #, ms
       start 2000 #, ms
        stop 8000 #, ms
       start 2000 #, ms
       stop
       ." demo complete. "
;

: timex 5000 #, for msec next ;

: noppp 1 #, drop ;

\ rp2040-sh_regForth-a/rp2040-sh_regForth-bb/main.fs

: id ." Sun 24 Dec 17:50:02 UTC 2023" cr
     ." zuma telfar kotari " cr
     ." rp2040" cr ;

turnkey decimal initGPIO interpret

\ END.
