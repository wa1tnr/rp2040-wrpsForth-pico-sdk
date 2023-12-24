\ main.fs
\  Sun 24 Dec 17:31:02 UTC 2023

\  was: Thu  6 Jan 17:55:54 UTC 2022

target
turnkey
    decimal
    initGPIO


: test  cr cr ." this is going to be good " cr
  ." meta-compilation in gforth " cr cr
  ."                               " cr
  ." A lot more confident now that " cr
  ." the memory footprint is (now) " cr
  ." reasonable (within design     " cr
  ." specification).               " cr
  ."                               " cr
  ." Keane Stanstussi Crayoti      " cr
  ."                               " cr
  ." 24 Dec 2023 17:31 UTC. " ;

: tsecxpj tusec ; \ wait 10 usec   - 10 microseconds
: tsecrrr msec ;  \ wait 1000 usec - 1 millisecond

: timed 10000 #, for tusec next ;

: blinque led on timed  led off timed timed timed ;

: demc $1B #, for blinque next ;

: noppp 1 #, drop ;

: id cr cr ." Sun 24 Dec 17:31:25 UTC 2023" cr
     ." dudatae hunarlp " cr
     ." rp2040" cr cr ;

turnkey decimal initGPIO interpret

\ END.
