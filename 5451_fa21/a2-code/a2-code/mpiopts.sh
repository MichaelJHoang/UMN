#!/bin/bash

if [[ -n "$DEBUG" ]]; then
    DEBUGOPTS="-x DEBUG"
fi

# export MPIOPTS="$DEBUGOPTS --mca opal_warn_on_missing_libcuda 0 --oversubscribe"
export MPIOPTS="$DEBUGOPTS --mca opal_warn_on_missing_libcuda 0"
