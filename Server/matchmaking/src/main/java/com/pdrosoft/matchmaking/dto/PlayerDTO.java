package com.pdrosoft.matchmaking.dto;

import lombok.Builder;
import lombok.Value;

@Value
@Builder
public class PlayerDTO {
    private Long id;

    private String username;
}
